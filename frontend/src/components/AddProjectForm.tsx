// src/components/AddProjectForm.tsx

import React, { useState } from "react";

interface ProjectFormData {
    name: string;
    clientName: string;
    projectValue: number;
    startDate: string;
    endDate?: string;
    managedByPartnerId: number;
}

const AddProjectForm: React.FC = () => {
    const [formData, setFormData] = useState<ProjectFormData>({
        name: "",
        clientName: "",
        projectValue: 0,
        startDate: "",
        endDate: "",
        managedByPartnerId: 0,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        console.log("Submit:", formData);
        // call your API here
    };

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                placeholder="Project Name"
            />
            <input
                type="text"
                name="clientName"
                value={formData.clientName}
                onChange={handleChange}
                placeholder="Client Name"
            />
            <input
                type="number"
                name="projectValue"
                value={formData.projectValue}
                onChange={handleChange}
                placeholder="Project Value"
            />
            <input
                type="date"
                name="startDate"
                value={formData.startDate}
                onChange={handleChange}
            />
            <input
                type="date"
                name="endDate"
                value={formData.endDate}
                onChange={handleChange}
            />
            <input
                type="number"
                name="managedByPartnerId"
                value={formData.managedByPartnerId}
                onChange={handleChange}
                placeholder="Partner ID"
            />
            <button type="submit">Add Project</button>
        </form>
    );
};

export default AddProjectForm;
