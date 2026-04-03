package stud.pmi31.pricelist.config;

import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.SecurityFilterChain;
import stud.pmi31.pricelist.dto.UserDto;
import stud.pmi31.pricelist.service.UserService;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;

@Configuration
@EnableWebSecurity
@RequiredArgsConstructor
public class SecurityConfig {
    
    private final UserService userService;

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
            .authorizeHttpRequests(auth -> auth
                .requestMatchers("/", "/home", "/products/search", "/categories/list", "/categories/view", "/register", "/css/**").permitAll()
                .requestMatchers("/categories/**", "/products/add", "/products/save",
                               "/products/move/**", "/products/delete/**", "/users/manage/**",
                               "/users/save").hasRole("ADMIN")
                .anyRequest().authenticated()
            )
            .formLogin(form -> form
                .loginPage("/login")
                .defaultSuccessUrl("/home", true)
                .failureUrl("/login?error=true")
                .permitAll()
            )
            .logout(logout -> logout
                .logoutSuccessUrl("/home")
                .permitAll()
            );
        return http.build();
    }
    
    @Bean
    public UserDetailsService userDetailsService() {
        return username -> {
            UserDto user = userService.findByLogin(username);
            if (user == null) {
                throw new UsernameNotFoundException("Пользователь не найден: " + username);
            }
            return org.springframework.security.core.userdetails.User.builder()
                    .username(user.getLogin())
                    .password(userService.getPasswordHash(user.getLogin()))
                    .roles(user.getRole() == 1 ? "ADMIN" : "USER")
                    .build();
        };
    }

}

