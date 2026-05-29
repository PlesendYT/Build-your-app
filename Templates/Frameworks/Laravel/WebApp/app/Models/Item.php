<?php

// Item Eloquent model for the web application
namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Item extends Model
{
    // Attributes that are mass assignable
    protected $fillable = ['title', 'description'];
}

// Routes file (routes/web.php)
// Route::get('/', [App\Http\Controllers\ItemController::class, 'index']);
// Route::get('/about', fn() => view('about'));
// Route::get('/contact', [App\Http\Controllers\ContactController::class, 'show']);
