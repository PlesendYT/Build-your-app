<?php

// Web routes for the Laravel web application
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ItemController;

// Home page displaying items list
Route::get('/', [ItemController::class, 'index'])->name('home');
// Static pages
Route::get('/about', fn() => view('about'))->name('about');
Route::get('/contact', fn() => view('contact'))->name('contact');
// Contact form submission
Route::post('/contact', [ItemController::class, 'store']);
