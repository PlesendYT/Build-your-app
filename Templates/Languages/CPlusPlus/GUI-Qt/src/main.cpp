#include <QApplication>
#include "mainwindow.hpp"

auto main(int argc, char* argv[]) -> int {
    QApplication app{argc, argv};
    MainWindow window;
    window.show();
    return app.exec();
}
