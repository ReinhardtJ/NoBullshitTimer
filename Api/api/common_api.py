from fastapi import FastAPI
from fastapi.responses import HTMLResponse


def add_index(api: FastAPI):
    @api.get("/", response_class=HTMLResponse)
    async def root():
        return HTMLResponse(status_code=200, content="""
    <h1>NoBullshitTimer API</h1>
    <p>To use the app, visit <a href="https://timer.reinhardt.ai">timer.reinhardt.ai</a></p>
    <p>To visit the API documentation, visit <a href="http://localhost:8000/docs">api.timer.reinhardt.ai</a></p>
    """)
