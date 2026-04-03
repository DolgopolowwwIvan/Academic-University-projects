package stud.pmi31.pricelist.dto;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;

@Data
public class CategoryDto {
    private Long id;
    
    @NotBlank(message = "Название категории обязательно")
    private String name;
    private String description;
    private Integer productCount;
}
