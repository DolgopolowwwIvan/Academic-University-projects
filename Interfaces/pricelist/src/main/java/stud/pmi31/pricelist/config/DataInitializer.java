package stud.pmi31.pricelist.config;

import lombok.RequiredArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import stud.pmi31.pricelist.model.Category;
import stud.pmi31.pricelist.model.Product;
import stud.pmi31.pricelist.model.User;
import stud.pmi31.pricelist.repository.CategoryRepository;
import stud.pmi31.pricelist.repository.ProductRepository;
import stud.pmi31.pricelist.repository.UserRepository;

import java.math.BigDecimal;

@Component
@RequiredArgsConstructor
public class DataInitializer implements CommandLineRunner {
    
    private final UserRepository userRepository;
    private final CategoryRepository categoryRepository;
    private final ProductRepository productRepository;
    private final PasswordEncoder passwordEncoder;
    
    @Override
    public void run(String... args) {
        // Создаем пользователей
        createUser("admin", "admin@pricelist.local", "admin", 1);
        createUser("user1", "user1@example.com", "user1", 0);
        createUser("user2", "user2@example.com", "user2", 0);
        
        // Создаем категории и товары
        Category electronics = createCategory("Электроника", "Телефоны, ноутбуки и аксессуары");
        Category clothing = createCategory("Одежда", "Одежда и обувь");
        Category food = createCategory("Продукты", "Продукты питания");
        
        // Товары для Электроники
        createProduct("iPhone-15", "iPhone 15 128GB", "Смартфон Apple iPhone 15", 
                      new BigDecimal("99999.00"), 10, electronics);
        createProduct("SAMSUNG-S24", "Samsung Galaxy S24", "Смартфон Samsung Galaxy S24", 
                      new BigDecimal("89999.00"), 15, electronics);
        createProduct("MACBOOK-AIR", "MacBook Air M2", "Ноутбук Apple MacBook Air 13\"", 
                      new BigDecimal("119999.00"), 8, electronics);
        createProduct("AIRPODS-PRO", "AirPods Pro 2", "Беспроводные наушники", 
                      new BigDecimal("24999.00"), 25, electronics);
        
        // Товары для Одежды
        createProduct("JACKET-001", "Зимняя куртка", "Тёплая зимняя куртка", 
                      new BigDecimal("15999.00"), 20, clothing);
        createProduct("JEANS-BLUE", "Джинсы Classic", "Мужские джинсы синие 32/32", 
                      new BigDecimal("4999.00"), 30, clothing);
        createProduct("SNEAKER-NIKE", "Кроссовки Nike Air", "Спортивные кроссовки 42 размер", 
                      new BigDecimal("12999.00"), 18, clothing);
        
        // Товары для Продуктов
        createProduct("COFFEE-ARABICA", "Кофе Арабика 1кг", "Молотый кофе 100% Арабика", 
                      new BigDecimal("1299.00"), 50, food);
        createProduct("CHOCOLATE-DARK", "Шоколад Тёмный", "Шоколад 85% какао 100г", 
                      new BigDecimal("199.00"), 100, food);
        createProduct("TEA-GREEN", "Чай Зелёный", "Зелёный чай в пакетиках 50 шт", 
                      new BigDecimal("299.00"), 75, food);
    }
    
    private User createUser(String login, String email, String password, int role) {
        if (userRepository.findByLogin(login).isEmpty()) {
            User user = new User();
            user.setLogin(login);
            user.setEmail(email);
            user.setPasswordHash(passwordEncoder.encode(password));
            user.setRole(role);
            userRepository.save(user);
            String roleText = role == 1 ? "администратор" : "пользователь";
            System.out.println("Создан " + roleText + ": login=" + login + ", password=" + password);
        }
        return userRepository.findByLogin(login).orElse(null);
    }
    
    private Category createCategory(String name, String description) {
        return categoryRepository.findByName(name)
                .orElseGet(() -> categoryRepository.save(new Category(null, name, description, null)));
    }
    
    private void createProduct(String sku, String name, String description, BigDecimal price, 
                               int stockQuantity, Category category) {
        if (productRepository.findBySku(sku).isEmpty()) {
            Product product = new Product();
            product.setSku(sku);
            product.setName(name);
            product.setDescription(description);
            product.setPrice(price);
            product.setStockQuantity(stockQuantity);
            product.setCategory(category);
            productRepository.save(product);
            System.out.println("Создан товар: " + name + " (" + sku + ")");
        }
    }
}
