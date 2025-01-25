from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
# from langchain.chains import SQLDatabaseChain
# from langchain.sql_database import SQLDatabase
# from langchain.llms import OpenAI

# Initialize FastAPI app
app = FastAPI()

# Define a Pydantic model for the input data
class QueryRequest(BaseModel):
    query: str

# Set up the database and LangChain
# db = SQLDatabase.from_uri("postgresql+psycopg2://user:password@localhost/dbname")
# llm = OpenAI(temperature=0)
# db_chain = SQLDatabaseChain(llm=llm, database=db, verbose=True)

@app.post("/process-query")
async def process_query(request: QueryRequest):
    try:
        # Generate SQL query and fetch results
        #results = db_chain.run(request.query)
        results = {"text": "ahoj"}
        return {"results": results}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

# Run the server (use `uvicorn app:app --reload` in terminal to start this)
