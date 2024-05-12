import React, { useState } from "react";

interface Message {
  text: string;
}

const ChatInterface: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([
    { text: "Hi, I am Drake Bell, your DB Chatbot for CRUD requests" },
  ]);
  const [inputText, setInputText] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (inputText.trim() === "") return;

    setMessages([...messages, { text: ". . ." }]);
    setLoading(true);

    // Simulate delay for accessing database
    await new Promise((resolve) => setTimeout(resolve, 1500));

    setMessages([...messages.slice(0, -1), { text: "Accessing database" }]);

    // Simulate delay for response from database
    await new Promise((resolve) => setTimeout(resolve, 2000));

    setMessages([...messages.slice(0, -1), { text: inputText }]);
    setInputText("");
    setLoading(false);
  };

  return (
    <div className="chat-container">
      <div className="chat-messages">
        {messages.map((message, index) => (
          <div key={index} className="chat-message">
            {message.text}
          </div>
        ))}
        {loading && <div className="chat-message">Accessing database...</div>}
      </div>
      <form onSubmit={handleSubmit} className="chat-input-form">
        <input
          type="text"
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          placeholder="Type your message..."
          className="chat-input"
          disabled={loading}
        />
        <button type="submit" className="send-button" disabled={loading}>
          Send
        </button>
      </form>
    </div>
  );
};

export default ChatInterface;
