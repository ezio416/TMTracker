﻿// c 2023-02-28
// m 2023-03-27

namespace TMT.Pages;

public partial class HomePage : ContentPage {
    public HomePage(HomeViewModel vm) {
        InitializeComponent();
        BindingContext = vm;
    }
}