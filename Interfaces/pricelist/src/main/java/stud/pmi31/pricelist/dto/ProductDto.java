package stud.pmi31.pricelist.dto;

import jakarta.validation.constraints.*;
import lombok.Data;
import java.math.BigDecimal;

@Data
public class ProductDto {
    private Long id;
    
    @NotBlank(message = "Артикул обязателен")
    private String sku;

    @NotBlank(message = "Название товара обязательно")
    private String name;
    private String description;
    
    @NotNull(message = "Цена обязательна")
    @DecimalMin(value = "0.01", message = "Цена должна быть больше 0")
    private BigDecimal price;
    
    @NotNull(message = "Количество на складе обязательно")
    @Min(value = 0, message = "Количество не может быть отрицательным")
    private Integer stockQuantity;
    
    @NotNull(message = "Категория обязательна")
    private Long categoryId;
    private String categoryName;
}
