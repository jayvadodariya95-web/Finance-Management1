import { useEffect, useState } from "react";
import type { ProjectDto, CreateProjectDto } from "../types";
import { getProjects, createProject } from "../api/projectApi";
import ProjectList from "../components/ProjectList";
import AddProjectForm from "../components/AddProjectForm";

const ProjectsPage = () => {
    const [projects, setProjects] = useState<ProjectDto[]>([]);
    const [showForm, setShowForm] = useState(false);

    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const data = await getProjects();
                setProjects(data);
            } catch (error) {
                console.error("Failed to load projects", error);
            }
        };

        fetchProjects();
    }, []);

    const handleAddProject = async (project: CreateProjectDto) => {
        try {
            await createProject(project);
            setShowForm(false);

            // Reload projects after add
            const data = await getProjects();
            setProjects(data);
        } catch (error) {
            console.error("Failed to add project", error);
        }
    };

    return (
        <div>
            <button onClick={() => setShowForm(true)}>
                Add Project
            </button>

            {showForm && (
                <AddProjectForm
                    onSubmit={handleAddProject}
                    onCancel={() => setShowForm(false)}
                />
            )}

            <ProjectList projects={projects} />
        </div>
    );
};

export default ProjectsPage;
