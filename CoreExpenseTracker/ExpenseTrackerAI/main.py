
import os

from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from groq import Groq

app = FastAPI()

# Connect to Groq using the environment variable
api_key = os.getenv("GROQ_API_KEY")
client = Groq(api_key=api_key) if api_key else None


# Home endpoint
@app.get("/")
def home():
    return {
        "message": "Expense Tracker AI service is running!"
    }


# Test the AI connection
@app.get("/test-ai")
def test_ai():
    if client is None:
        raise HTTPException(
            status_code=500,
            detail="GROQ_API_KEY is not configured."
        )

    try:
        response = client.chat.completions.create(
            model="openai/gpt-oss-20b",
            messages=[
                {
                    "role": "user",
                    "content": (
                        "Write one friendly sentence welcoming "
                        "a user to an expense tracker."
                    )
                }
            ]
        )

        return {
            "summary": response.choices[0].message.content
        }

    except Exception as e:
        print("GROQ ERROR:", repr(e))

        raise HTTPException(
            status_code=502,
            detail="Groq request failed. Check the Python terminal."
        )


# Data received for monthly analysis
class MonthlyExpenseData(BaseModel):
    month: str
    income: float
    expenses: float
    categories: dict[str, float]


# Generate an AI monthly expense summary
@app.post("/monthly-summary")
def monthly_summary(data: MonthlyExpenseData):
    if client is None:
        raise HTTPException(
            status_code=500,
            detail="GROQ_API_KEY is not configured."
        )

    balance = data.income - data.expenses

    prompt = f"""
    Analyze the following monthly financial data.

    Month: {data.month}
    Total income: ₹{data.income:.2f}
    Total expenses: ₹{data.expenses:.2f}
    Remaining balance: ₹{balance:.2f}
    Expenses by category: {data.categories}

    Write a friendly, clear monthly spending summary.

    Explain the main spending patterns and provide
    two practical suggestions to help manage expenses.

    Use only the information provided.
    Do not invent comparisons with previous months.
    Do not claim the remaining balance is savings unless
    all relevant financial commitments are accounted for.

    Keep the response concise and easy to understand.
    """

    try:
        response = client.chat.completions.create(
            model="openai/gpt-oss-20b",
            messages=[
                {
                    "role": "system",
                    "content": (
                        "You are a friendly personal finance "
                        "assistant. Explain financial information "
                        "in simple language."
                    )
                },
                {
                    "role": "user",
                    "content": prompt
                }
            ]
        )

        summary = response.choices[0].message.content

        return {
            "month": data.month,
            "summary": summary
        }

    except Exception as e:
        print("GROQ ERROR:", repr(e))

        raise HTTPException(
            status_code=502,
            detail="Could not generate the monthly summary."
        )
