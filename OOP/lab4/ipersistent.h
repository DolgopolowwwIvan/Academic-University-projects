#ifndef IPERSISTENT_H
#define IPERSISTENT_H

#include <string>

class IPersistent {
public:
    virtual ~IPersistent() = default;
    
    // Чисто виртуальные функции для персистентности
    virtual void save_to_file(const std::string& filename) const = 0;
    virtual void load_from_file(const std::string& filename) = 0;
};

#endif