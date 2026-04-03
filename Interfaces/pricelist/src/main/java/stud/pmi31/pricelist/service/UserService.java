package stud.pmi31.pricelist.service;

import stud.pmi31.pricelist.dto.UserDto;
import stud.pmi31.pricelist.model.User;
import stud.pmi31.pricelist.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class UserService {
    
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    
    private static final String DEFAULT_PASSWORD = "12345";
    
    public List<UserDto> findAll() {
        return userRepository.findAll().stream()
                .map(this::toDto)
                .collect(Collectors.toList());
    }
    
    public UserDto findById(Long id) {
        return userRepository.findById(id)
                .map(this::toDto)
                .orElse(null);
    }
    
    public UserDto findByLogin(String login) {
        return userRepository.findByLogin(login)
                .map(this::toDto)
                .orElse(null);
    }
    
    @Transactional
    public UserDto save(UserDto dto) {
        User user;
        boolean isNewUser = false;
        
        if (dto.getId() != null) {
            user = userRepository.findById(dto.getId())
                    .orElseThrow(() -> new IllegalArgumentException("Пользователь не найден"));
        } else {
            // Проверка на существование пользователя с таким логином или email
            if (userRepository.existsByLogin(dto.getLogin())) {
                throw new IllegalArgumentException("Пользователь с таким логином уже существует");
            }
            if (userRepository.existsByEmail(dto.getEmail())) {
                throw new IllegalArgumentException("Пользователь с таким email уже существует");
            }
            user = new User();
            user.setPasswordHash(passwordEncoder.encode(DEFAULT_PASSWORD));
            isNewUser = true;
        }
        
        user.setLogin(dto.getLogin());
        user.setEmail(dto.getEmail());
        user.setRole(dto.getRole());
        
        return toDto(userRepository.save(user));
    }
    
    @Transactional
    public void delete(Long id) {
        userRepository.deleteById(id);
    }
    
    public boolean isAdmin(String login) {
        return userRepository.findByLogin(login)
                .map(u -> u.getRole() == 1)
                .orElse(false);
    }
    
    @Transactional
    public UserDto register(UserDto dto) {
        // Проверка на существование пользователя с таким логином
        if (userRepository.existsByLogin(dto.getLogin())) {
            throw new IllegalArgumentException("Пользователь с таким логином уже существует");
        }
        // Проверка на существование пользователя с таким email
        if (userRepository.existsByEmail(dto.getEmail())) {
            throw new IllegalArgumentException("Пользователь с таким email уже существует");
        }

        User user = new User();
        user.setLogin(dto.getLogin());
        user.setEmail(dto.getEmail());
        user.setRole(0); // Обычный пользователь по умолчанию
        user.setPasswordHash(passwordEncoder.encode(dto.getPassword()));

        return toDto(userRepository.save(user));
    }

    public String getPasswordHash(String login) {
        return userRepository.findByLogin(login)
                .map(User::getPasswordHash)
                .orElse("");
    }

    private UserDto toDto(User user) {
        UserDto dto = new UserDto();
        dto.setId(user.getId());
        dto.setLogin(user.getLogin());
        dto.setEmail(user.getEmail());
        dto.setRole(user.getRole());
        dto.setRoleName(user.getRole() == 1 ? "Администратор" : "Пользователь");
        return dto;
    }
}
