package stud.pmi31.pricelist.controller;

import stud.pmi31.pricelist.dto.ProductDto;
import stud.pmi31.pricelist.dto.CategoryDto;
import stud.pmi31.pricelist.service.ProductService;
import stud.pmi31.pricelist.service.CategoryService;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import java.math.BigDecimal;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Controller
@RequiredArgsConstructor
public class ProductController {
    
    private final ProductService productService;
    private final CategoryService categoryService;
    
    @GetMapping("/products/search")
    public String searchPage(Model model) {
        model.addAttribute("categories", categoryService.findAll());
        return "products-search";
    }
    
    @PostMapping("/products/search")
    @ResponseBody
    public List<ProductDto> searchProducts(
            @RequestParam(required = false) String productName,
            @RequestParam(required = false) Long categoryId,
            @RequestParam(required = false) BigDecimal priceFrom,
            @RequestParam(required = false) BigDecimal priceTo,
            @RequestParam(required = false) Boolean inStockOnly) {
        
        return productService.search(productName, categoryId, priceFrom, priceTo, inStockOnly);
    }
    
    @GetMapping("/products/add")
    public String addProductPage(Model model) {
        List<CategoryDto> categories = categoryService.findAll();
        if (categories.isEmpty()) {
            model.addAttribute("noCategories", true);
        } else {
            model.addAttribute("noCategories", false);
        }
        model.addAttribute("categories", categories);
        model.addAttribute("product", new ProductDto());
        return "products-add";
    }
    
    @PostMapping("/products/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> saveProduct(@Valid @ModelAttribute ProductDto product, 
                                                            BindingResult result) {
        Map<String, Object> response = new HashMap<>();
        
        if (result.hasErrors()) {
            response.put("success", false);
            StringBuilder errors = new StringBuilder();
            result.getFieldErrors().forEach(error -> 
                errors.append(error.getDefaultMessage()).append("; ")
            );
            response.put("message", errors.toString().trim());
            return ResponseEntity.badRequest().body(response);
        }
        
        // Проверка уникальности названия товара в категории
        if (productService.existsByNameAndCategoryId(product.getName(), product.getCategoryId(), product.getId())) {
            response.put("success", false);
            response.put("message", "Товар с таким названием уже существует в выбранной категории");
            return ResponseEntity.badRequest().body(response);
        }
        
        try {
            ProductDto saved = productService.save(product);
            response.put("success", true);
            response.put("message", "Товар успешно сохранён");
            response.put("product", saved);
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при сохранении: " + e.getMessage());
        }
        
        return ResponseEntity.ok(response);
    }
    
    @GetMapping("/products/move/{id}")
    public String moveProductPage(@PathVariable Long id, Model model) {
        ProductDto product = productService.findById(id);
        if (product == null) {
            return "redirect:/products/search?error=notfound";
        }
        model.addAttribute("product", product);
        model.addAttribute("categories", categoryService.findAll());
        return "products-move";
    }
    
    @PostMapping("/products/move")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> moveProduct(
            @RequestParam Long productId,
            @RequestParam Long targetCategoryId) {
        
        Map<String, Object> response = new HashMap<>();
        
        try {
            productService.moveToCategory(productId, targetCategoryId);
            response.put("success", true);
            response.put("message", "Товар успешно перемещён");
        } catch (IllegalArgumentException e) {
            response.put("success", false);
            response.put("message", e.getMessage());
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при перемещении: " + e.getMessage());
        }
        
        return ResponseEntity.ok(response);
    }
    
    @PostMapping("/products/delete/{id}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> deleteProduct(@PathVariable Long id) {
        Map<String, Object> response = new HashMap<>();
        try {
            productService.delete(id);
            response.put("success", true);
            response.put("message", "Товар удалён");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при удалении: " + e.getMessage());
        }
        return ResponseEntity.ok(response);
    }

    @GetMapping("/products/edit/{id}")
    @ResponseBody
    public ResponseEntity<ProductDto> getProduct(@PathVariable Long id) {
        ProductDto product = productService.findById(id);
        if (product == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(product);
    }

    @GetMapping("/api/check-product-sku")
    @ResponseBody
    public Map<String, Boolean> checkProductSku(@RequestParam String sku,
                                                 @RequestParam(required = false) Long excludeId) {
        Map<String, Boolean> result = new HashMap<>();
        boolean exists = productService.existsBySku(sku, excludeId);
        result.put("exists", exists);
        return result;
    }

    @GetMapping("/api/check-product-name")
    @ResponseBody
    public Map<String, Boolean> checkProductName(@RequestParam String name,
                                                  @RequestParam Long categoryId,
                                                  @RequestParam(required = false) Long excludeId) {
        Map<String, Boolean> result = new HashMap<>();
        boolean exists = productService.existsByNameAndCategoryId(name, categoryId, excludeId);
        result.put("exists", exists);
        return result;
    }
}
