def execute_chat_gpt():
    import openai

    key = ''
    with open('..\OpenAI.API.key', 'r', encoding="utf-8-sig") as file:
        key = file.read().rstrip()

    openai.api_key = key
    response = openai.Completion.create(
        engine="davinci",
        prompt="Hallo A.I.S.A!",
        max_tokens=150
    )

    print (response)

execute_chat_gpt()