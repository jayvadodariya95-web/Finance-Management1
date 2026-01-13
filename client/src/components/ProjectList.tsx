import type { ProjectDto } from "../types";

interface ProjectListProps {
    projects: ProjectDto[];
}

const ProjectList = ({ projects }: ProjectListProps) => {
    return (
        <div>
            <h2>Projects</h2>

            {projects.length === 0 && <p>No projects found.</p>}

            <ul>
                {projects.map(project => (
                    <li key={project.id}>
                        <strong>{project.name}</strong> — {project.clientName}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ProjectList;
