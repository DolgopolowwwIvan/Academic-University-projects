package stud.pmi31.pricelist.repository;

import stud.pmi31.pricelist.model.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;

public interface ProductRepository extends JpaRepository<Product, Long> {
    
    // Поиск по названию товара в пределах категории (для уникальности)
    Optional<Product> findByNameAndCategoryId(String name, Long categoryId);
    
    boolean existsByNameAndCategoryId(String name, Long categoryId);
    
    // Поиск товаров с фильтрами
    @Query("SELECT p FROM Product p JOIN FETCH p.category WHERE " +
           "(:productName IS NULL OR LOWER(p.name) LIKE LOWER(CONCAT('%', :productName, '%'))) AND " +
           "(:categoryId IS NULL OR p.category.id = :categoryId) AND " +
           "(:priceFrom IS NULL OR p.price >= :priceFrom) AND " +
           "(:priceTo IS NULL OR p.price <= :priceTo) AND " +
           "(:inStockOnly IS NULL OR :inStockOnly = false OR p.stockQuantity > 0)")
    List<Product> searchProducts(
            @Param("productName") String productName,
            @Param("categoryId") Long categoryId,
            @Param("priceFrom") BigDecimal priceFrom,
            @Param("priceTo") BigDecimal priceTo,
            @Param("inStockOnly") Boolean inStockOnly
    );
    
    List<Product> findByCategoryId(Long categoryId);
}
