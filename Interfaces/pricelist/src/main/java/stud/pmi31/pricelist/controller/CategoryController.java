package stud.pmi31.pricelist.controller;

import stud.pmi31.pricelist.dto.CategoryDto;
import stud.pmi31.pricelist.dto.ProductDto;
import stud.pmi31.pricelist.service.CategoryService;
import stud.pmi31.pricelist.service.ProductService;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Controller
@RequiredArgsConstructor
public class CategoryController {
    
    private final CategoryService categoryService;
    private final ProductService productService;

    @GetMapping("/categories")
    public String categoriesPage(Model model) {
        model.addAttribute("categories", categoryService.findAllWithProductCount());
        model.addAttribute("category", new CategoryDto());
        return "categories";
    }
    
    @GetMapping("/categories/view")
    public String viewCategoriesPage(Model model) {
        model.addAttribute("categories", categoryService.findAllWithProductCount());
        return "categories-view";
    }

    @GetMapping("/categories/{id}/products")
    public String viewCategoryProducts(@PathVariable Long id, Model model) {
        CategoryDto category = categoryService.findById(id);
        if (category == null) {
            return "redirect:/categories/view?error=notfound";
        }
        model.addAttribute("category", category);
        model.addAttribute("products", productService.findByCategoryId(id));
        return "category-products";
    }

    @GetMapping("/categories/list")
    @ResponseBody
    public List<CategoryDto> getCategoriesList() {
        return categoryService.findAll();
    }
    
    @GetMapping("/categories/products/{id}")
    @ResponseBody
    public List<ProductDto> getCategoryProducts(@PathVariable Long id) {
        return productService.findByCategoryId(id);
    }

    @PostMapping("/categories/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> saveCategory(@Valid @ModelAttribute CategoryDto category, 
                                                             BindingResult result) {
        Map<String, Object> response = new HashMap<>();
        
        if (result.hasErrors()) {
            response.put("success", false);
            response.put("message", result.getFieldError().getDefaultMessage());
            return ResponseEntity.badRequest().body(response);
        }
        
        // Проверка уникальности названия
        if (categoryService.existsByNameExcludingId(category.getName(), category.getId())) {
            response.put("success", false);
            response.put("message", "Категория с таким названием уже существует");
            return ResponseEntity.badRequest().body(response);
        }
        
        try {
            CategoryDto saved = categoryService.save(category);
            response.put("success", true);
            response.put("message", "Категория успешно сохранена");
            response.put("category", saved);
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при сохранении: " + e.getMessage());
        }
        
        return ResponseEntity.ok(response);
    }
    
    @GetMapping("/categories/edit/{id}")
    @ResponseBody
    public ResponseEntity<CategoryDto> getCategory(@PathVariable Long id) {
        CategoryDto category = categoryService.findById(id);
        if (category == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(category);
    }
    
    @PostMapping("/categories/delete/{id}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> deleteCategory(@PathVariable Long id) {
        Map<String, Object> response = new HashMap<>();
        try {
            categoryService.delete(id);
            response.put("success", true);
            response.put("message", "Категория удалена");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при удалении: " + e.getMessage());
        }
        return ResponseEntity.ok(response);
    }
}
