import React, { useState } from "react";
import type{ CreateProjectDto } from "../types";

interface AddProjectFormProps {
    onSubmit: (project: CreateProjectDto) => void;
    onCancel: () => void;
}

const AddProjectForm: React.FC<AddProjectFormProps> = ({ onSubmit, onCancel }) => {
    const [project, setProject] = useState<CreateProjectDto>({
        name: "",
        description: "",
        clientName: "",
        projectValue: 0,
        startDate: new Date().toISOString().split("T")[0],
        endDate: "",
        managedByPartnerId: 1, // default partnerId
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setProject({ ...project, [e.target.name]: e.target.value });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(project);
    };

    return (
        <div className="p-4 border rounded bg-gray-50">
            <h2 className="text-xl font-bold mb-4">Add Project</h2>
            <form onSubmit={handleSubmit} className="space-y-2">
                <div>
                    <label>Name</label>
                    <input name="name" value={project.name} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div>
                    <label>Description</label>
                    <textarea name="description" value={project.description} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div>
                    <label>Client Name</label>
                    <input name="clientName" value={project.clientName} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div>
                    <label>Project Value</label>
                    <input type="number" name="projectValue" value={project.projectValue} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div>
                    <label>Start Date</label>
                    <input type="date" name="startDate" value={project.startDate} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div>
                    <label>End Date</label>
                    <input type="date" name="endDate" value={project.endDate} onChange={handleChange} className="border p-1 w-full" />
                </div>
                <div className="flex gap-2 mt-2">
                    <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">Save</button>
                    <button type="button" onClick={onCancel} className="bg-gray-500 text-white px-4 py-2 rounded">Cancel</button>
                </div>
            </form>
        </div>
    );
};

export default AddProjectForm;
