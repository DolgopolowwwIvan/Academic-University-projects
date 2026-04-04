package stud.pmi31.pricelist.service;

import stud.pmi31.pricelist.dto.CategoryDto;
import stud.pmi31.pricelist.model.Category;
import stud.pmi31.pricelist.repository.CategoryRepository;
import stud.pmi31.pricelist.repository.ProductRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class CategoryService {
    
    private final CategoryRepository categoryRepository;
    private final ProductRepository productRepository;

    @Transactional(readOnly = true)
    public List<CategoryDto> findAll() {
        return categoryRepository.findAll().stream()
                .map(this::toDtoWithCount)
                .collect(Collectors.toList());
    }

    public List<CategoryDto> findAllWithProductCount() {
        return categoryRepository.findAll().stream()
                .map(category -> {
                    CategoryDto dto = toDto(category);
                    dto.setProductCount((int) productRepository.countByCategoryId(category.getId()));
                    return dto;
                })
                .collect(Collectors.toList());
    }
    
    public CategoryDto findById(Long id) {
        return categoryRepository.findById(id)
                .map(this::toDto)
                .orElse(null);
    }
    
    @Transactional
    public CategoryDto save(CategoryDto dto) {
        Category category;
        if (dto.getId() != null) {
            category = categoryRepository.findById(dto.getId())
                    .orElseThrow(() -> new IllegalArgumentException("Категория не найдена"));
            category.setName(dto.getName());
            category.setDescription(dto.getDescription());
        } else {
            category = toEntity(dto);
        }
        return toDto(categoryRepository.save(category));
    }
    
    @Transactional
    public void delete(Long id) {
        categoryRepository.deleteById(id);
    }
    
    public boolean existsByName(String name) {
        return categoryRepository.existsByName(name);
    }
    
    public boolean existsByNameExcludingId(String name, Long excludeId) {
        return categoryRepository.findByName(name)
                .map(c -> !c.getId().equals(excludeId))
                .orElse(false);
    }
    
    private CategoryDto toDto(Category category) {
        CategoryDto dto = new CategoryDto();
        dto.setId(category.getId());
        dto.setName(category.getName());
        dto.setDescription(category.getDescription());
        return dto;
    }
    
    private CategoryDto toDtoWithCount(Category category) {
        CategoryDto dto = toDto(category);
        dto.setProductCount((int) productRepository.countByCategoryId(category.getId()));
        return dto;
    }

    private Category toEntity(CategoryDto dto) {
        Category category = new Category();
        category.setName(dto.getName());
        category.setDescription(dto.getDescription());
        return category;
    }
}
