<?php

// Item Eloquent model for the REST API
namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Item extends Model
{
    // Attributes that are mass assignable
    protected $fillable = ['title', 'description'];
}
