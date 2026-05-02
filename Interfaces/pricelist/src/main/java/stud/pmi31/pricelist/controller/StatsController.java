package stud.pmi31.pricelist.controller;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import stud.pmi31.pricelist.repository.CategoryRepository;
import stud.pmi31.pricelist.repository.ProductRepository;
import stud.pmi31.pricelist.repository.UserRepository;

import java.util.HashMap;
import java.util.Map;

@RestController
@RequiredArgsConstructor
public class StatsController {

    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    private final UserRepository userRepository;

    @GetMapping("/api/stats")
    public Map<String, Long> getStats() {
        Map<String, Long> stats = new HashMap<>();
        stats.put("products", productRepository.count());
        stats.put("categories", categoryRepository.count());
        stats.put("users", userRepository.count());
        return stats;
    }
}
