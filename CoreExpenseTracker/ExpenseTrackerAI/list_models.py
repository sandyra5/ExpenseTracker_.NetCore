
import os
from groq import Groq

api_key = os.getenv("GROQ_API_KEY")

if not api_key:
    print("Groq API key is missing.")
else:
    client = Groq(api_key=api_key)
    models = client.models.list()

    print("Models available to your account:\n")

    for model in models.data:
        print(model.id)
