<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Item extends Model
{
    protected $fillable = ['title', 'description'];
}

// Routes file (routes/web.php)
// Route::get('/', [App\Http\Controllers\ItemController::class, 'index']);
// Route::get('/about', fn() => view('about'));
// Route::get('/contact', [App\Http\Controllers\ContactController::class, 'show']);
