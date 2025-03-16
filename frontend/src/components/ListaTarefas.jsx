import React, { useState, useEffect } from 'react';
import api from '../services/api';
import formatarData from '../helpers/dateHelper';
import { Table, Button, Form, Row, Col } from 'react-bootstrap';
import { Link } from 'react-router';
import 'bootstrap/dist/css/bootstrap.min.css';

const ListaTarefas = () => {
    const [tarefas, setTarefas] = useState([]);
    const [filtroStatus, setFiltroStatus] = useState('');

    useEffect(() => {
      api.obterTarefas(filtroStatus)
        .then(result => {
          console.log('buscou tarefas', filtroStatus)
          setTarefas(result)
        })
        .catch(erro => {
          if (erro?.response?.data?.errors)
              alert(Object.values(erro.response.data.errors).flat())
          else
              alert("Ocorreu um erro ao comunicar com o servidor. Tente novamente mais tarde.")
      });
    }, [filtroStatus]);

    const obterTarefas = async () => {
        const tarefa = await api.obterTarefas(filtroStatus);
        setTarefas(tarefa);
    };

    const apagarTarefa = async (id) => {
        await api.excluirTarefa(id);
        obterTarefas();
    };

    const tratarStatus = (status) => {
      switch (status) {
        case "EmProgresso":
          return "Em progresso";
        case "Concluida":
          return "Concluída"
        default:
          return status;
      }
    }

    return (
        <div>
            <h1>Lista de Tarefas</h1>
            <Link to="/tarefa/criar" className="btn btn-success gap-2 mb-2">Nova Tarefa</Link>
            <Form.Group as={Row} >
                <Form.Label column md="auto" className="mb-2" >Filtrar por Status:</Form.Label>
                <Col md="auto">
                  <Form.Control as="select" value={filtroStatus} onChange={(e) => setFiltroStatus(e.target.value)}>
                    <option value="">Todos</option>
                    <option value="Pendente">Pendente</option>
                    <option value="EmProgresso">Em Progresso</option>
                    <option value="Concluida">Concluída</option>
                  </Form.Control>
                </Col>
            </Form.Group>
            <Table responsive striped bordered hover>
                <thead>
                    <tr>
                        <th>Título</th>
                        <th>Descrição</th>
                        <th>Data criação</th>
                        <th>Data Conclusão</th>
                        <th>Status</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {tarefas.map(tarefa => (
                        <tr key={tarefa.id}>
                            <td>{tarefa.titulo}</td>
                            <td>{tarefa.descricao}</td>
                            <td>{formatarData(tarefa.dataCriacao)}</td>
                            <td>{formatarData(tarefa.dataConclusao)}</td>
                            <td>{tratarStatus(tarefa.status)}</td>
                            <td>
                                <div className="d-flex gap-2 mb-2">
                                  <Link to={`/tarefa/editar/${tarefa.id}`} className="btn btn-primary">Editar</Link>
                                  <Button variant="danger" onClick={() => apagarTarefa(tarefa.id)}>Excluir</Button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default ListaTarefas;