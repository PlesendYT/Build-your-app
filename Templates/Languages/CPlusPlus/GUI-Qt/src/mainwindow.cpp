#include "mainwindow.hpp"

MainWindow::MainWindow(QWidget* parent)
    : QWidget{parent} {
    auto* layout = new QVBoxLayout{this};

    auto* title = new QLabel{"Hello, Qt6!"};
    title->setStyleSheet("font-size: 18pt;");
    layout->addWidget(title);

    entry_ = new QLineEdit{};
    entry_->setPlaceholderText("Enter text...");
    layout->addWidget(entry_);

    label_ = new QLabel{"Count: 0"};
    layout->addWidget(label_);

    button_ = new QPushButton{"Click me"};
    layout->addWidget(button_);

    connect(button_, &QPushButton::clicked, this, &MainWindow::onButtonClicked);

    setLayout(layout);
    setWindowTitle("Qt6 Template");
    resize(400, 300);
}

void MainWindow::onButtonClicked() {
    ++counter_;
    label_->setText(QString{"Count: %1"}.arg(counter_));
}
