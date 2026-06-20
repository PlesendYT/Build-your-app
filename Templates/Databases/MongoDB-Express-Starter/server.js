const express = require("express");
const mongoose = require("mongoose");

async function main() {
  await mongoose.connect(process.env.MONGO_URL);
  const Item = mongoose.model("Item", new mongoose.Schema({ name: { type: String, required: true } }));
  const app = express();
  app.use(express.json());
  app.get("/health", (_req, res) => res.json({ ok: true }));
  app.get("/items", async (_req, res) => res.json(await Item.find()));
  app.post("/items", async (req, res) => res.status(201).json(await Item.create(req.body)));
  app.listen(3000, () => console.log("listening on :3000"));
}
main().catch((err) => { console.error(err); process.exit(1); });