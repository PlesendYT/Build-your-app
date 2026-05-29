<?php

// REST API routes for the Item resource
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\ItemController;

// Register standard RESTful API routes for items
Route::apiResource('items', ItemController::class);
