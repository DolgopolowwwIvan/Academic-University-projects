#ifndef DATA_EXPORTER_H
#define DATA_EXPORTER_H

#include <string>
#include <fstream>

class data_exporter {
private:
    std::string filename;

public:
    data_exporter(const std::string& filename);

    void export_data(const double* x_values,
        const double* density_values,
        int size);
};

#endif