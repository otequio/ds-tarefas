import React from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router';
import ListaTarefas from './components/ListaTarefas';
import FormularioTarefa from './components/FormularioTarefa';
import './App.css';

const router = createBrowserRouter([
  {path: "/", element: <ListaTarefas/>},
  {path: "/tarefa/criar", element: <FormularioTarefa/>},
  {path: "/tarefa/editar/:id", element: <FormularioTarefa/>}
]);

const App = () => {
  return (
      <div>
        <RouterProvider router={router}/>
      </div>
      
  );
};

export default App;