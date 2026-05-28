#pragma once
#include <QLabel>
#include <QLineEdit>
#include <QPushButton>
#include <QVBoxLayout>
#include <QWidget>

class MainWindow : public QWidget {
    Q_OBJECT
public:
    explicit MainWindow(QWidget* parent = nullptr);

private slots:
    void onButtonClicked();

private:
    QLineEdit* entry_;
    QLabel* label_;
    QPushButton* button_;
    int counter_ = 0;
};
