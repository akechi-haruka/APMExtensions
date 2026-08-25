import re

def on_page_markdown(markdown, **kwargs):
    return re.sub(r'\[(.*?)\]\(([^.]*?)\)', r'[\1](\2.md)', markdown)