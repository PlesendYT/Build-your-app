// MainWindow header - Qt6 widget with label, input, button, and click counter.
#pragma once
#include <QLabel>
#include <QLineEdit>
#include <QPushButton>
#include <QVBoxLayout>
#include <QWidget>

// Main application window with a counter button and text input.
class MainWindow : public QWidget {
    Q_OBJECT
public:
    explicit MainWindow(QWidget* parent = nullptr);

private slots:
    // Increment counter and update display.
    void onButtonClicked();

private:
    QLineEdit* entry_;
    QLabel* label_;
    QPushButton* button_;
    int counter_ = 0;
};
