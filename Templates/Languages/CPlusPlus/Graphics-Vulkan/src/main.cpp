// Vulkan application template - creates instance, device, and window (C++).
#include <GLFW/glfw3.h>
#include <vulkan/vulkan.h>
#include <algorithm>
#include <cstdint>
#include <iostream>
#include <limits>
#include <optional>
#include <set>
#include <span>
#include <stdexcept>
#include <vector>

// Vulkan application with window, instance, and logical device.
class VulkanApp {
public:
    // Initialize window and Vulkan.
    VulkanApp() {
        init_window();
        init_vulkan();
    }

    // Clean up Vulkan and GLFW resources.
    ~VulkanApp() {
        vkDestroyDevice(device_, nullptr);
        vkDestroyInstance(instance_, nullptr);
        glfwDestroyWindow(window_);
        glfwTerminate();
    }

    // Main loop: poll events until window closes.
    void run() {
        while (!glfwWindowShouldClose(window_)) {
            glfwPollEvents();
        }
    }

private:
    GLFWwindow* window_ = nullptr;
    VkInstance instance_ = VK_NULL_HANDLE;
    VkDevice device_ = VK_NULL_HANDLE;
    VkPhysicalDevice physical_device_ = VK_NULL_HANDLE;

    // Create a GLFW window without OpenGL context.
    void init_window() {
        glfwInit();
        glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
        window_ = glfwCreateWindow(800, 600, "Vulkan Template", nullptr, nullptr);
    }

    // Set up Vulkan instance, physical device, and logical device.
    void init_vulkan() {
        create_instance();
        pick_physical_device();
        create_logical_device();
    }

    // Create a Vulkan instance with GLFW extensions.
    void create_instance() {
        VkApplicationInfo app_info{};
        app_info.sType = VK_STRUCTURE_TYPE_APPLICATION_INFO;
        app_info.pApplicationName = "Vulkan Template";
        app_info.applicationVersion = VK_MAKE_VERSION(1, 0, 0);
        app_info.pEngineName = "No Engine";
        app_info.engineVersion = VK_MAKE_VERSION(1, 0, 0);
        app_info.apiVersion = VK_API_VERSION_1_3;

        uint32_t glfw_count;
        auto glfw_extensions = glfwGetRequiredInstanceExtensions(&glfw_count);
        std::vector<const char*> extensions(glfw_extensions,
            glfw_extensions + glfw_count);

        VkInstanceCreateInfo create_info{};
        create_info.sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        create_info.pApplicationInfo = &app_info;
        create_info.enabledExtensionCount = static_cast<uint32_t>(extensions.size());
        create_info.ppEnabledExtensionNames = extensions.data();

        if (vkCreateInstance(&create_info, nullptr, &instance_) != VK_SUCCESS) {
            throw std::runtime_error("Failed to create Vulkan instance");
        }
    }

    // Select the first available Vulkan physical device.
    void pick_physical_device() {
        uint32_t count = 0;
        vkEnumeratePhysicalDevices(instance_, &count, nullptr);
        if (count == 0) throw std::runtime_error("No Vulkan devices");
        std::vector<VkPhysicalDevice> devices(count);
        vkEnumeratePhysicalDevices(instance_, &count, devices.data());
        physical_device_ = devices[0];

        VkPhysicalDeviceProperties props;
        vkGetPhysicalDeviceProperties(physical_device_, &props);
        std::cout << "Using: " << props.deviceName << "\n";
    }

    // Create a logical device from the selected physical device.
    void create_logical_device() {
        uint32_t count = 0;
        vkGetPhysicalDeviceQueueFamilyProperties(physical_device_, &count, nullptr);
        std::vector<VkQueueFamilyProperties> queues(count);
        vkGetPhysicalDeviceQueueFamilyProperties(physical_device_, &count, queues.data());

        float priority = 1.0f;
        VkDeviceQueueCreateInfo queue_info{};
        queue_info.sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO;
        queue_info.queueFamilyIndex = 0;
        queue_info.queueCount = 1;
        queue_info.pQueuePriorities = &priority;

        VkPhysicalDeviceFeatures features{};
        VkDeviceCreateInfo create_info{};
        create_info.sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
        create_info.pQueueCreateInfos = &queue_info;
        create_info.queueCreateInfoCount = 1;
        create_info.pEnabledFeatures = &features;

        if (vkCreateDevice(physical_device_, &create_info, nullptr, &device_) != VK_SUCCESS) {
            throw std::runtime_error("Failed to create logical device");
        }
    }
};

// Entry point: create and run the Vulkan application.
int main() {
    try {
        VulkanApp app;
        app.run();
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << "\n";
        return 1;
    }
    return 0;
}
