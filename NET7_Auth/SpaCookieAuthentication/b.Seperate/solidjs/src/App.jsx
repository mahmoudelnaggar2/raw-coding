import logo from './logo.svg';
import styles from './App.module.css';

function App() {
  const login = () => fetch('https://localhost:7278/api/login', {method: 'post', credentials: 'include'})
  const test = () => fetch('https://localhost:7278/api/test', {credentials: 'include'})
  return (
    <div class={styles.App}>
      <button onClick={login}>Login</button>
      <button onClick={test}>Test</button>
    </div>
  );
}

export default App;
