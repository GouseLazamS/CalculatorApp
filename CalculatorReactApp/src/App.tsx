import { useState } from 'react';
import axios from 'axios'; // Now we will use this!
import './App.css';

function App() {
  const [v1, setV1] = useState<number>(0);
  const [v2, setV2] = useState<number>(0);
  const [result, setResult] = useState<number | string>(0);
  const [status, setStatus] = useState<string>('Ready');

  // Ensure this port matches your .NET project's HTTPS port
  // const API_URL = 'https://localhost:7169/api/Calculator/calculate';

  // Realigned for DevOps: Pulls from .env file or defaults to localhost
const API_URL = import.meta.env.VITE_API_URL || 'https://localhost:7169/api/Calculator/calculate';

const onCalculate = async (type: string) => {
  setStatus('Processing...');
  
  // Create the expression string automatically for the backend
  const expression = `${v1} ${type} ${v2}`;

  try {
    // Realigned to match your Swagger URL exactly:
    // https://localhost:7169/api/Calculator/calculate?expression=add&v1=10&v2=10&type=add
    const response = await axios.post(
      `${API_URL}?expression=${encodeURIComponent(expression)}&v1=${v1}&v2=${v2}&type=${type}`
    );

    setResult(response.data.result);
    setStatus('Success - Logged to SQL');
  } catch (error: any) {
    setResult('Error');
    setStatus(error.response?.data || 'API Error: Check CORS');
  }
};

  return (
    <div className="calc-container">
      <h1 style={{ color: '#646cff' }}>React Sci-Calc</h1>
      <p style={{ fontSize: '12px', opacity: 0.7 }}>Vite + .NET 8 + MS SQL</p>
      
      <div className="input-group">
        <input 
          type="number" 
          value={v1} 
          onChange={(e) => setV1(Number(e.target.value))} 
          placeholder="Value 1" 
        />
        <input 
          type="number" 
          value={v2} 
          onChange={(e) => setV2(Number(e.target.value))} 
          placeholder="Value 2" 
        />
      </div>

      <div className="button-grid">
        {/* Standard Operations */}
        <button onClick={() => onCalculate('add')}>+</button>
        <button onClick={() => onCalculate('subtract')}>-</button>
        <button onClick={() => onCalculate('multiply')}>*</button>
        <button onClick={() => onCalculate('divide')}>/</button>
        
        {/* Scientific Operations */}
        <button className="sci" onClick={() => onCalculate('pow')}>x^y</button>
        <button className="sci" onClick={() => onCalculate('sqrt')}>√x</button>
        <button className="sci" onClick={() => onCalculate('sin')}>Sin</button>
        <button className="sci" onClick={() => onCalculate('cos')}>Cos</button>
      </div>

      <div className="display-panel">
        <h2 style={{ margin: 0 }}>Result: <span className="res-text">{result}</span></h2>
        <p style={{ marginTop: '10px', fontSize: '14px' }}>
          <b>Status:</b> {status}
        </p>
      </div>
    </div>
  );
}

export default App;