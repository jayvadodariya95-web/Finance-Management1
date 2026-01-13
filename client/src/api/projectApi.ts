import axios from 'axios';
import type{ ApiResponse, ProjectDto, CreateProjectDto } from '../types';

const API_URL = "https://localhost:62843/api";

export const getProjects = async () => {
    const response = await axios.get<ApiResponse<ProjectDto[]>>(
        `${API_URL}/Projects`
    );
    return response.data.data;
};

export const createProject = async (project: CreateProjectDto) => {
    const response = await axios.post<ApiResponse<ProjectDto>>(
        `${API_URL}/Projects`,
        project
    );
    return response.data.data;
};

// 2. Export as an object (Keeps App.tsx working)
export const api = {
    getProjects,
    createProject
};