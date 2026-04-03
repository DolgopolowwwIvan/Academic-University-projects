package stud.pmi31.pricelist.service;

import stud.pmi31.pricelist.dto.ProductDto;
import stud.pmi31.pricelist.model.Product;
import stud.pmi31.pricelist.model.Category;
import stud.pmi31.pricelist.repository.ProductRepository;
import stud.pmi31.pricelist.repository.CategoryRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.math.BigDecimal;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {
    
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    
    public List<ProductDto> findAll() {
        return productRepository.findAll().stream()
                .map(this::toDto)
                .collect(Collectors.toList());
    }
    
    public List<ProductDto> findByCategoryId(Long categoryId) {
        return productRepository.findByCategoryId(categoryId).stream()
                .map(this::toDto)
                .collect(Collectors.toList());
    }
    
    public ProductDto findById(Long id) {
        return productRepository.findById(id)
                .map(this::toDto)
                .orElse(null);
    }
    
    public List<ProductDto> search(String productName, Long categoryId, 
                                    BigDecimal priceFrom, BigDecimal priceTo, 
                                    Boolean inStockOnly) {
        return productRepository.searchProducts(
                productName, categoryId, priceFrom, priceTo, inStockOnly
        ).stream()
                .map(this::toDto)
                .collect(Collectors.toList());
    }
    
    @Transactional
    public ProductDto save(ProductDto dto) {
        Product product;
        if (dto.getId() != null) {
            product = productRepository.findById(dto.getId())
                    .orElseThrow(() -> new IllegalArgumentException("Товар не найден"));
            product.setName(dto.getName());
            product.setDescription(dto.getDescription());
            product.setPrice(dto.getPrice());
            product.setStockQuantity(dto.getStockQuantity());
            if (dto.getCategoryId() != null) {
                Category category = categoryRepository.findById(dto.getCategoryId())
                        .orElseThrow(() -> new IllegalArgumentException("Категория не найдена"));
                product.setCategory(category);
            }
        } else {
            product = toEntity(dto);
        }
        return toDto(productRepository.save(product));
    }
    
    @Transactional
    public void delete(Long id) {
        productRepository.deleteById(id);
    }
    
    @Transactional
    public ProductDto moveToCategory(Long productId, Long newCategoryId) {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new IllegalArgumentException("Товар не найден"));
        
        Category newCategory = categoryRepository.findById(newCategoryId)
                .orElseThrow(() -> new IllegalArgumentException("Категория не найдена"));
        
        if (product.getCategory().getId().equals(newCategoryId)) {
            throw new IllegalArgumentException("Товар уже находится в выбранной категории");
        }
        
        product.setCategory(newCategory);
        return toDto(productRepository.save(product));
    }
    
    public boolean existsByNameAndCategoryId(String name, Long categoryId, Long excludeId) {
        if (excludeId != null) {
            return productRepository.findByNameAndCategoryId(name, categoryId)
                    .map(p -> !p.getId().equals(excludeId))
                    .orElse(false);
        }
        return productRepository.existsByNameAndCategoryId(name, categoryId);
    }
    
    private ProductDto toDto(Product product) {
        ProductDto dto = new ProductDto();
        dto.setId(product.getId());
        dto.setName(product.getName());
        dto.setDescription(product.getDescription());
        dto.setPrice(product.getPrice());
        dto.setStockQuantity(product.getStockQuantity());
        dto.setCategoryId(product.getCategory().getId());
        dto.setCategoryName(product.getCategory().getName());
        return dto;
    }
    
    private Product toEntity(ProductDto dto) {
        Product product = new Product();
        product.setName(dto.getName());
        product.setDescription(dto.getDescription());
        product.setPrice(dto.getPrice());
        product.setStockQuantity(dto.getStockQuantity());
        
        if (dto.getCategoryId() != null) {
            Category category = categoryRepository.findById(dto.getCategoryId())
                    .orElseThrow(() -> new IllegalArgumentException("Категория не найдена"));
            product.setCategory(category);
        }
        return product;
    }
}
