#include "data_exporter.h"
#include <iostream>

data_exporter::data_exporter(const std::string& filename)
    : filename(filename) {
}

void data_exporter::export_data(const double* x_values,
    const double* density_values,
    int size) {
    std::ofstream file(filename);
    if (!file.is_open()) {
        std::cerr << "Error: Cannot open file " << filename << " for writing" << std::endl;
        return;
    }

    for (int i = 0; i < size; ++i) {
        file << x_values[i] << " " << density_values[i] << "\n";
    }

    file.close();
}