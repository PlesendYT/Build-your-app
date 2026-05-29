# Defines the application routes for the Rails web app
Rails.application.routes.draw do
  # RESTful routes for the items resource
  resources :items
  # Set the root path to the items index action
  root "items#index"
end
