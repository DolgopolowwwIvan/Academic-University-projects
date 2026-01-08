#ifndef IDISTRIBUTION_H
#define IDISTRIBUTION_H

class IDistribution {
public:
    virtual ~IDistribution() = default;
    
    // Чисто виртуальные функции
    virtual double density(double x) const = 0;
    virtual double expectation() const = 0;
    virtual double variance() const = 0;
    virtual double skewness() const = 0;
    virtual double kurtosis() const = 0;
    virtual double generate() const = 0;
    virtual bool is_valid() const = 0;
};

#endif