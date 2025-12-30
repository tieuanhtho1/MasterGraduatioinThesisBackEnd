using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.BusinessLogic.Mindmap;
using WebAPI.Models.DTOs.MindMap;
using WebAPI.Services.User;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MindMapController : ControllerBase
    {
        private readonly IMindMapBusinessLogic _mindMapBusinessLogic;
        private readonly IUserService _userService;

        public MindMapController(IMindMapBusinessLogic mindMapBusinessLogic, IUserService userService)
        {
            _mindMapBusinessLogic = mindMapBusinessLogic;
            _userService = userService;
        }

        private async Task<int> GetUserId()
        {
            // Try to get user ID from claims first
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            // If that fails, try to get username and look up the user ID
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
                throw new UnauthorizedAccessException("User information not found in token");

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            return user.Id;
        }

        // ============ MindMap Endpoints ============

        /// <summary>
        /// Get all mindmaps for the current user
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<MindMapResponseDto>>> GetUserMindMaps([FromQuery] int userId)
        {
            try
            {
                var mindMaps = await _mindMapBusinessLogic.GetUserMindMapsAsync(userId);
                return Ok(mindMaps);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving mindmaps", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific mindmap by ID (without full node details)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MindMapResponseDto>> GetMindMapById(int id)
        {
            try
            {
                var userId = await GetUserId();
                var mindMap = await _mindMapBusinessLogic.GetMindMapByIdAsync(id, userId);

                if (mindMap == null)
                    return NotFound(new { message = "MindMap not found" });

                return Ok(mindMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the mindmap", error = ex.Message });
            }
        }

        /// <summary>
        /// Get full mindmap with all nodes and flashcard information
        /// This is the main endpoint for displaying the mindmap in the frontend
        /// </summary>
        [HttpGet("{id}/full")]
        public async Task<ActionResult<FullMindMapResponseDto>> GetFullMindMap(int id)
        {
            try
            {
                var userId = await GetUserId();
                var mindMap = await _mindMapBusinessLogic.GetFullMindMapAsync(id, userId);

                if (mindMap == null)
                    return NotFound(new { message = "MindMap not found" });

                return Ok(mindMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the full mindmap", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new mindmap
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MindMapResponseDto>> CreateMindMap([FromBody] CreateMindMapDto dto)
        {
            try
            {
                var userId = await GetUserId();
                var mindMap = await _mindMapBusinessLogic.CreateMindMapAsync(dto, userId);
                return CreatedAtAction(nameof(GetMindMapById), new { id = mindMap.Id }, mindMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the mindmap", error = ex.Message });
            }
        }

        /// <summary>
        /// Update a mindmap's basic information (name, description)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MindMapResponseDto>> UpdateMindMap(int id, [FromBody] UpdateMindMapDto dto)
        {
            try
            {
                var userId = await GetUserId();
                var mindMap = await _mindMapBusinessLogic.UpdateMindMapAsync(id, dto, userId);

                if (mindMap == null)
                    return NotFound(new { message = "MindMap not found" });

                return Ok(mindMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the mindmap", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a mindmap and all its nodes
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMindMap(int id)
        {
            try
            {
                var userId = await GetUserId();
                var result = await _mindMapBusinessLogic.DeleteMindMapAsync(id, userId);

                if (!result)
                    return NotFound(new { message = "MindMap not found" });

                return Ok(new { message = "MindMap deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the mindmap", error = ex.Message });
            }
        }

        // ============ MindMapNode Endpoints ============

        /// <summary>
        /// Get a specific node by ID
        /// </summary>
        [HttpGet("nodes/{nodeId}")]
        public async Task<ActionResult<MindMapNodeResponseDto>> GetNodeById(int nodeId)
        {
            try
            {
                var userId = await GetUserId();
                var node = await _mindMapBusinessLogic.GetNodeByIdAsync(nodeId, userId);

                if (node == null)
                    return NotFound(new { message = "Node not found" });

                return Ok(node);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the node", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new node in a mindmap
        /// </summary>
        [HttpPost("{mindMapId}/nodes")]
        public async Task<ActionResult<MindMapNodeResponseDto>> CreateNode(int mindMapId, [FromBody] CreateMindMapNodeDto dto)
        {
            try
            {
                var userId = await GetUserId();
                var node = await _mindMapBusinessLogic.CreateNodeAsync(mindMapId, dto, userId);
                return CreatedAtAction(nameof(GetNodeById), new { nodeId = node.Id }, node);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the node", error = ex.Message });
            }
        }

        /// <summary>
        /// Update a node's properties (position, color, hideChildren, parent)
        /// </summary>
        [HttpPut("nodes/{nodeId}")]
        public async Task<ActionResult<MindMapNodeResponseDto>> UpdateNode(int nodeId, [FromBody] UpdateMindMapNodeDto dto)
        {
            try
            {
                var userId = await GetUserId();
                var node = await _mindMapBusinessLogic.UpdateNodeAsync(nodeId, dto, userId);

                if (node == null)
                    return NotFound(new { message = "Node not found" });

                return Ok(node);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the node", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a node (children become root nodes)
        /// </summary>
        [HttpDelete("nodes/{nodeId}")]
        public async Task<ActionResult> DeleteNode(int nodeId)
        {
            try
            {
                var userId = await GetUserId();
                var result = await _mindMapBusinessLogic.DeleteNodeAsync(nodeId, userId);

                if (!result)
                    return NotFound(new { message = "Node not found" });

                return Ok(new { message = "Node deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the node", error = ex.Message });
            }
        }
    }
}
