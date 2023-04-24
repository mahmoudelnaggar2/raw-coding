import logo from './logo.svg';
import './App.css';
import user from './init-user'

function App() {
    const login = () => fetch('/api/login', {method: 'post'})
    const test = () => fetch('/api/test')
    return (
        <div className="App">
            <button onClick={login}>Login</button>
            <button onClick={test}>Test</button>
            <span>{user['username']}</span>
        </div>
    );
}

export default App;
