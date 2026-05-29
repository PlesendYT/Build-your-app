<?php

// Post Eloquent model with category and comments relationships
namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Post extends Model
{
    // Attributes that are mass assignable
    protected $fillable = ['title', 'content', 'category_id'];

    // A post belongs to a single category
    public function category() {
        return $this->belongsTo(Category::class);
    }

    // A post has many comments
    public function comments() {
        return $this->hasMany(Comment::class);
    }
}
