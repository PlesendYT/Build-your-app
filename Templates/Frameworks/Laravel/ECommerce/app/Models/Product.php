<?php

// Product Eloquent model for the E-Commerce application
namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Product extends Model
{
    // Attributes that are mass assignable
    protected $fillable = ['name', 'slug', 'description', 'price', 'category_id', 'stock'];
}
