"""RAG pipeline: document loading, embedding, retrieval, and generation."""

from __future__ import annotations

from pathlib import Path

import faiss
import numpy as np
from sentence_transformers import SentenceTransformer
from transformers import pipeline

# Load the embedding model for converting text to vectors
# All-MiniLM-L6-v2 is lightweight and works well for semantic search
embedder = SentenceTransformer("all-MiniLM-L6-v2")

# Load a lightweight generation model for answering based on context
generator = pipeline("text2text-generation", model="google/flan-t5-small")

# Global index and document store
index: faiss.IndexFlatL2 | None = None
documents: list[str] = []


def load_documents(doc_dir: str = "documents") -> list[str]:
    """Read all .txt files from the given directory."""
    docs = []
    for filepath in Path(doc_dir).glob("*.txt"):
        docs.append(filepath.read_text(encoding="utf-8").strip())
    return docs


def build_index(docs: list[str]) -> faiss.IndexFlatL2:
    """Embed documents and build a FAISS index for similarity search."""
    embeddings = embedder.encode(docs, convert_to_numpy=True)
    dimension = embeddings.shape[1]
    idx = faiss.IndexFlatL2(dimension)
    idx.add(embeddings)
    return idx


def retrieve(query: str, k: int = 2) -> list[str]:
    """Retrieve the top-k most relevant documents for a query."""
    query_vec = embedder.encode([query], convert_to_numpy=True)
    distances, indices = index.search(query_vec, k)  # noqa: F841
    return [documents[i] for i in indices[0]]


def generate(query: str, context: str) -> str:
    """Generate an answer using the LLM with retrieved context."""
    prompt = f"Answer based on the context.\nContext: {context}\nQuestion: {query}\nAnswer:"
    result = generator(prompt, max_length=128, do_sample=False)
    return result[0]["generated_text"]


def main() -> None:
    """Run the full RAG pipeline: load, index, retrieve, generate."""
    global index, documents

    # Step 1: load documents from the documents directory
    documents = load_documents()

    if not documents:
        print("No documents found. Add .txt files to the documents/ folder.")
        return

    print(f"Loaded {len(documents)} document(s).")

    # Step 2: build the FAISS vector index
    index = build_index(documents)
    print("Index built successfully.\n")

    # Step 3: interactive query loop
    while True:
        query = input("Query: ")
        if query.lower() in {"quit", "exit"}:
            break

        # Step 4: retrieve relevant context
        context_chunks = retrieve(query)
        context = "\n".join(context_chunks)

        # Step 5: generate answer with context
        answer = generate(query, context)
        print(f"Answer: {answer}\n")


if __name__ == "__main__":
    main()
