// MongoDB CRUD example using Mongoose ODM
// Run: npm install && npm start

const mongoose = require("mongoose");

// Define a User schema with validation rules
const userSchema = new mongoose.Schema({
  name: { type: String, required: true },
  email: { type: String, required: true, unique: true },
  age: { type: Number, min: 0 },
  createdAt: { type: Date, default: Date.now },
});

// Create the User model from the schema
const User = mongoose.model("User", userSchema);

async function main() {
  // Connect to MongoDB running in Docker
  await mongoose.connect(process.env.MONGODB_URI || "mongodb://localhost:27017/myapp");
  console.log("Connected to MongoDB");

  // CREATE: Insert a new user document
  const alice = await User.create({
    name: "Alice",
    email: "alice@example.com",
    age: 30,
  });
  console.log("Created:", alice);

  // READ: Find all users in the collection
  const users = await User.find();
  console.log("All users:", users);

  // UPDATE: Change Alice's age
  const updated = await User.findOneAndUpdate(
    { name: "Alice" },
    { age: 31 },
    { new: true }
  );
  console.log("Updated:", updated);

  // DELETE: Remove Alice from the collection
  const deleted = await User.findOneAndDelete({ name: "Alice" });
  console.log("Deleted:", deleted);

  // Close the Mongoose connection
  await mongoose.disconnect();
}

main().catch(console.error);
