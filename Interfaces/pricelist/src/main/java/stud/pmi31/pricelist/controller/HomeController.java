package stud.pmi31.pricelist.controller;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class HomeController {
    
    @GetMapping("/")
    public String root() {
        return "redirect:/home";
    }
    
    @GetMapping("/home")
    public String home(Model model, Authentication auth) {
        boolean isAdmin = auth != null && auth.getAuthorities().contains(new SimpleGrantedAuthority("ROLE_ADMIN"));
        model.addAttribute("isAdmin", isAdmin);
        model.addAttribute("username", auth != null ? auth.getName() : "Гость");
        return "home";
    }
    
    @GetMapping("/login")
    public String login() {
        return "login";
    }
}
