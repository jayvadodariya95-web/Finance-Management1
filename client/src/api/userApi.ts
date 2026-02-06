import axios from 'axios';
import type { ApiResponse, UserDto, CreateUserDto } from '../types';

const API_URL = "https://localhost:62843/api";

export const getUsers = async () => {
    const response = await axios.get<ApiResponse<UserDto[]>>(
        `${API_URL}/Users`
    );
    return response.data.data;
};

export const createUser = async (user: CreateUserDto) => {
    const response = await axios.post<ApiResponse<UserDto>>(
        `${API_URL}/Users`,
        user
    );
    return response.data.data;
};

// 2. Export as an object (Keeps App.tsx working)
export const api = {
    getUsers,
    createUser
};