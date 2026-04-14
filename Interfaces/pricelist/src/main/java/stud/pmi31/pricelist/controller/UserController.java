package stud.pmi31.pricelist.controller;

import stud.pmi31.pricelist.dto.UserDto;
import stud.pmi31.pricelist.service.UserService;
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
public class UserController {
    
    private final UserService userService;
    
    @GetMapping("/users/manage")
    public String manageUsersPage(Model model) {
        model.addAttribute("users", userService.findAll());
        model.addAttribute("user", new UserDto());
        return "users-manage";
    }
    
    @GetMapping("/users/list")
    @ResponseBody
    public List<UserDto> getUsersList() {
        return userService.findAll();
    }

    @GetMapping("/api/check-login")
    @ResponseBody
    public Map<String, Boolean> checkLogin(@RequestParam String login,
                                           @RequestParam(required = false) Long excludeId) {
        Map<String, Boolean> result = new HashMap<>();
        boolean exists = userService.existsByLogin(login);
        if (excludeId != null) {
            UserDto user = userService.findByLogin(login);
            exists = user != null && !user.getId().equals(excludeId);
        }
        result.put("exists", exists);
        return result;
    }

    @GetMapping("/api/check-email")
    @ResponseBody
    public Map<String, Boolean> checkEmail(@RequestParam String email,
                                           @RequestParam(required = false) Long excludeId) {
        Map<String, Boolean> result = new HashMap<>();
        boolean exists = userService.existsByEmail(email);
        if (excludeId != null) {
            UserDto user = userService.findByEmail(email);
            exists = user != null && !user.getId().equals(excludeId);
        }
        result.put("exists", exists);
        return result;
    }

    @PostMapping("/users/save")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> saveUser(@Valid @ModelAttribute UserDto user, 
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
        
        // Проверка: если обновляем существующего пользователя
        if (user.getId() != null) {
            UserDto existing = userService.findById(user.getId());
            if (existing != null) {
                // Проверка уникальности email (кроме текущего пользователя)
                UserDto byEmail = userService.findByLogin(user.getLogin()); // Тут логика может быть другой
            }
        }
        
        try {
            UserDto saved = userService.save(user);
            response.put("success", true);
            response.put("message", user.getId() != null ? "Роль пользователя обновлена" : "Пользователь создан с паролем 12345");
            response.put("user", saved);
        } catch (IllegalArgumentException e) {
            response.put("success", false);
            response.put("message", e.getMessage());
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при сохранении: " + e.getMessage());
        }
        
        return ResponseEntity.ok(response);
    }
    
    @GetMapping("/users/edit/{id}")
    @ResponseBody
    public ResponseEntity<UserDto> getUser(@PathVariable Long id) {
        UserDto user = userService.findById(id);
        if (user == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(user);
    }
    
    @PostMapping("/users/delete/{id}")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> deleteUser(@PathVariable Long id) {
        Map<String, Object> response = new HashMap<>();
        try {
            userService.delete(id);
            response.put("success", true);
            response.put("message", "Пользователь удалён");
        } catch (Exception e) {
            response.put("success", false);
            response.put("message", "Ошибка при удалении: " + e.getMessage());
        }
        return ResponseEntity.ok(response);
    }

    @GetMapping("/register")
    public String registerPage(Model model) {
        model.addAttribute("user", new UserDto());
        return "register";
    }

    @PostMapping("/register")
    public String register(@ModelAttribute UserDto user,
                          @RequestParam String confirmPassword,
                          Model model) {
        // Валидация
        if (user.getLogin() == null || user.getLogin().trim().length() < 3) {
            model.addAttribute("error", "Логин должен содержать минимум 3 символа");
            return "register";
        }

        if (user.getPassword() == null || user.getPassword().length() < 4) {
            model.addAttribute("error", "Пароль должен содержать минимум 4 символа");
            return "register";
        }

        if (!user.getPassword().equals(confirmPassword)) {
            model.addAttribute("error", "Пароли не совпадают");
            return "register";
        }

        if (user.getEmail() == null || !user.getEmail().contains("@")) {
            model.addAttribute("error", "Введите корректный email");
            return "register";
        }

        try {
            user.setRole(0); // Обычный пользователь
            userService.register(user);
            model.addAttribute("success", "Регистрация успешна! Теперь вы можете войти.");
            return "redirect:/login?registered";
        } catch (IllegalArgumentException e) {
            model.addAttribute("error", e.getMessage());
            return "register";
        }
    }
}
