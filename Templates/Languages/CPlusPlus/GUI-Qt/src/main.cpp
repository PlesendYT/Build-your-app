// Qt6 application entry point - creates and shows the MainWindow.
#include <QApplication>
#include "mainwindow.hpp"

// Entry point: initialize Qt application and show window.
auto main(int argc, char* argv[]) -> int {
    QApplication app{argc, argv};
    MainWindow window;
    window.show();
    return app.exec();
}
