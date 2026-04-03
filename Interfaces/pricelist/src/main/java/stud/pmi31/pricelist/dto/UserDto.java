package stud.pmi31.pricelist.dto;

import jakarta.validation.constraints.*;
import lombok.Data;

@Data
public class UserDto {
    private Long id;
    
    @NotBlank(message = "Логин обязателен")
    private String login;
    
    @NotBlank(message = "Email обязателен")
    @Email(message = "Некорректный формат email")
    private String email;
    
    @NotNull(message = "Роль обязательна")
    private Integer role;
    
    private String roleName;

    private String password;
}
